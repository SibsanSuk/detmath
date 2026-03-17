# detmath

Deterministic fixed-point math library for city-building and RTS games in C#.
ไลบรารีเลขคณิตแบบ fixed-point สำหรับเกมสร้างเมืองและ RTS — ผลลัพธ์เหมือนกันทุก platform

Targets `netstandard2.0` — drop the DLL into Unity and start writing simulation logic immediately.
รองรับ `netstandard2.0` — เอา DLL วางใน Unity แล้วเริ่มเขียน simulation logic ได้ทันที

All arithmetic uses `long` integers. No `float`. No `double`. Same result on every platform.
การคำนวณทั้งหมดใช้ `long` integer เท่านั้น ไม่มี `float` หรือ `double` — ได้ผลเหมือนกันทุก platform

## Installation / การติดตั้ง

Build and reference the DLL directly:
Build แล้ว reference DLL ตรงๆ:

```bash
dotnet build src/DetMath/DetMath.csproj -c Release
```

Then add a reference to `DetMath.dll` in your Unity project under `Assets/Plugins/`.
จากนั้นนำ `DetMath.dll` ไปวางใน `Assets/Plugins/` ของ Unity project

## Quick Start

```csharp
using DetMath;

Fix64 taxRate    = Fix64.FromRatio(15, 100);  // อัตราภาษี 0.15
Fix64 population = Fix64.FromInt(12_000);     // ประชากร 12,000
Fix64 income     = population * taxRate;      // รายได้ภาษี 1800.00

Fix64 happiness = DetMathf.Clamp(
    Fix64.FromRatio(13, 10),  // 1.30 — ค่าดิบจาก simulation (เกิน 1)
    Fix64.Zero,
    Fix64.One);               // clamp เหลือ 1.00

GridPoint warehouse = new(3, 7);
GridPoint market    = new(8, 2);
int travelCost      = warehouse.ManhattanDistance(market);  // 10 tile
```

## API Reference

### `Fix64` — fixed-point value type / ชนิดข้อมูลทศนิยม deterministic

2 decimal places. Safe multiplication range: ±30,370,004.
ความละเอียด 2 ทศนิยม — ช่วงที่คูณได้ปลอดภัย: ±30,370,004

```csharp
// Factory — สร้างค่า
Fix64.FromInt(5)           // 5.00
Fix64.FromRatio(3, 2)      // 1.50
Fix64.FromRaw(150)         // 1.50 (raw internal value)

// Conversion — แปลงค่า
value.ToIntFloor()         // floor ลงหา -∞   (-1.50 → -2)
value.ToIntTruncate()      // ตัดทิ้งทศนิยม   (-1.50 → -1)
value.ToString()           // "1.50"

// Operators: + - * / == != < > <= >=

// Constants — ค่าคงที่
Fix64.Zero   // 0.00
Fix64.One    // 1.00
Fix64.Half   // 0.50
```

> `ToIntFloor` และ `ToIntTruncate` ต่างกันเฉพาะค่าลบที่มีเศษทศนิยม เช่น `-1.50 → -2` (floor) vs `-1` (truncate)

### `DetMathf` — math functions / ฟังก์ชันคณิตศาสตร์

Mirrors Unity `Mathf`. All functions operate on `Fix64`.
ออกแบบให้เรียกใช้เหมือน Unity `Mathf` — ฟังก์ชันทั้งหมดรับและคืน `Fix64`

```csharp
DetMathf.Abs(value)               // ค่าสัมบูรณ์
DetMathf.Min(a, b)                // ค่าน้อยกว่า
DetMathf.Max(a, b)                // ค่ามากกว่า
DetMathf.Clamp(value, min, max)   // จำกัดให้อยู่ใน [min, max]
DetMathf.Lerp(from, to, t)        // interpolate เชิงเส้น, t ถูก clamp อยู่ที่ [0, 1]
DetMathf.Ratio(n, d)              // shorthand ของ Fix64.FromRatio(n, d)
```

### `GridPoint` — integer tile coordinate / พิกัด tile บน grid

```csharp
GridPoint a = new(3, 7);
GridPoint b = new(8, 2);

a.ManhattanDistance(b)   // 10 — ระยะทาง 4 ทิศ เหมาะกับ road grid
a.ChebyshevDistance(b)   //  5 — ระยะทาง 8 ทิศ เหมาะกับ unit ที่เดินทแยงได้

a + new GridPoint(1, 0)  // (4, 7) — เลื่อนตำแหน่ง
a - b                    // (-5, 5)
```

### `DetVector2` — 2D vector / เวกเตอร์ 2 มิติ

```csharp
DetVector2 a = new(Fix64.FromInt(3), Fix64.FromInt(4));
DetVector2 b = new(Fix64.One, Fix64.Zero);

DetVector2.Dot(a, b)    // 3.00 — dot product
a.LengthSquared         // 25.00 — ความยาวยกกำลังสอง

a + b                   // (4.00, 4.00)
a * Fix64.Half          // (1.50, 2.00) — scale
```

## Examples

### Economy / เศรษฐกิจ

```csharp
Fix64 population = Fix64.FromInt(12_000);
Fix64 taxPerHead = Fix64.FromRatio(15, 100);  // ภาษีหัวละ 0.15 ต่อเดือน
Fix64 upkeep     = Fix64.FromRatio(3, 2);     // ค่าดูแลหัวละ 1.50

Fix64 income  = population * taxPerHead;  // รายได้ 1800.00
Fix64 expense = population * upkeep;      // ค่าใช้จ่าย 18000.00
Fix64 deficit = income - expense;         // ขาดดุล -16200.00
```

### Production / การผลิต

```csharp
Fix64 outputPerFactory = Fix64.FromRatio(5, 2);                    // ผลิต 2.50 หน่วย/tick
Fix64 totalOutput      = Fix64.FromInt(8) * outputPerFactory;      // โรงงาน 8 แห่ง = 20.00

Fix64 effectiveLoad = totalOutput * Fix64.FromRatio(3, 4);  // รับได้ 75% = 15.00
```

### Supply Chain / โลจิสติกส์

```csharp
// คำนวณเวลาเดินทาง: ระยะทาง 14 tile ที่ความเร็ว 1.50 tile/tick
Fix64 ticks = Fix64.FromInt(14) / Fix64.FromRatio(3, 2);  // 9.33 tick

// smooth speed boost ด้วย Lerp
Fix64 actual = DetMathf.Lerp(Fix64.FromInt(2), Fix64.FromInt(5), Fix64.FromRatio(1, 3));  // 3.00
```

### Grid & Distance / ระยะทางบน grid

```csharp
GridPoint depot   = new(3, 7);
GridPoint outpost = new(8, 2);

// ถนน (เดินแค่ 4 ทิศ)
int roadTiles = depot.ManhattanDistance(outpost);  // 10 tile

// unit เดินได้ 8 ทิศ (รวม diagonal)
int moveTiles = depot.ChebyshevDistance(outpost);  //  5 tile
```

### Unit Facing / ทิศทาง unit (RTS)

```csharp
DetVector2 facing  = new(Fix64.One, Fix64.Zero);                          // unit หันขวา
DetVector2 toEnemy = new(Fix64.FromRatio(4, 5), Fix64.FromRatio(3, 5));   // ทิศสู่ศัตรู

Fix64 alignment = DetVector2.Dot(facing, toEnemy);  // 0.80 — หันเข้าหาศัตรูเกือบตรง
```

## Porting Guide / คู่มือแปลงโค้ดเดิม

แปลงโค้ดที่ใช้ `float` / `double` มาเป็น detmath — เหมาะสำหรับทั้งนักพัฒนาและ AI agent ที่ช่วย migrate codebase

### ชนิดข้อมูล

| เดิม | detmath | หมายเหตุ |
| --- | --- | --- |
| `float` / `double` | `Fix64` | ใช้แทนกันได้ทุกกรณีใน simulation logic |
| `Vector2` | `DetVector2` | ใช้ `Fix64` แทน `float` ใน component |
| `Vector2Int` / `(int x, int y)` | `GridPoint` | สำหรับพิกัด tile บน grid |

### การสร้างค่า

```csharp
// ❌ เดิม
float rate = 0.15f;
float speed = 1.5f;
float whole = 12f;

// ✅ detmath
Fix64 rate  = Fix64.FromRatio(15, 100);  // 0.15 — ใช้ ratio เสมอ ไม่ใช้ literal ทศนิยม
Fix64 speed = Fix64.FromRatio(3, 2);     // 1.50
Fix64 whole = Fix64.FromInt(12);         // 12.00
```

> อย่าแปลงด้วย `Fix64.FromRaw` ยกเว้นรู้ค่า raw จริงๆ — ใช้ `FromRatio` หรือ `FromInt` เสมอ

### Arithmetic

```csharp
// ❌ เดิม
float income  = population * taxRate;
float balance = income - upkeep;
float half    = value / 2f;

// ✅ detmath — syntax เหมือนเดิมทุกอย่าง แค่เปลี่ยน type
Fix64 income  = population * taxRate;
Fix64 balance = income - upkeep;
Fix64 half    = value / Fix64.FromInt(2);
```

### Math functions

```csharp
// ❌ เดิม
Mathf.Abs(value)
Mathf.Min(a, b)
Mathf.Max(a, b)
Mathf.Clamp(value, 0f, 1f)
Mathf.Lerp(from, to, t)

// ✅ detmath — เปลี่ยนแค่ prefix จาก Mathf เป็น DetMathf
DetMathf.Abs(value)
DetMathf.Min(a, b)
DetMathf.Max(a, b)
DetMathf.Clamp(value, Fix64.Zero, Fix64.One)
DetMathf.Lerp(from, to, t)
```

### Vector

```csharp
// ❌ เดิม
Vector2 velocity = new Vector2(3f, 4f);
Vector2 slowed   = velocity * 0.5f;
float   dot      = Vector2.Dot(a, b);
float   sqLen    = velocity.sqrMagnitude;

// ✅ detmath
DetVector2 velocity = new(Fix64.FromInt(3), Fix64.FromInt(4));
DetVector2 slowed   = velocity * Fix64.Half;
Fix64      dot      = DetVector2.Dot(a, b);
Fix64      sqLen    = velocity.LengthSquared;
```

### Grid distance

```csharp
// ❌ เดิม — มักใช้ Mathf.Abs บวกกันเอง
int manhattan = Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
int chebyshev = Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));

// ✅ detmath
int manhattan = a.ManhattanDistance(b);  // 4-directional (road)
int chebyshev = a.ChebyshevDistance(b);  // 8-directional (unit movement)
```

### แปลงค่ากลับเป็น int เพื่อ index หรือ loop

```csharp
// ❌ เดิม
int cycles = Mathf.FloorToInt(elapsed);  // floor ลงหา -∞

// ✅ detmath
int cycles = elapsed.ToIntFloor();       // floor ลงหา -∞ (เหมือนกัน)
int cycles = elapsed.ToIntTruncate();    // ตัดทิ้งทศนิยม — ใช้เมื่อต้องการ truncate
```

### Clamp01

```csharp
// ❌ เดิม
float clamped = Mathf.Clamp01(value);

// ✅ detmath
Fix64 clamped = DetMathf.Clamp(value, Fix64.Zero, Fix64.One);
```

### Pattern ที่พบบ่อยใน simulation

```csharp
// คำนวณ efficiency จาก ratio แล้ว clamp ไว้ที่ [0, 1]
// ❌ เดิม
float efficiency = Mathf.Clamp01((float)workers / requiredWorkers);

// ✅ detmath
Fix64 efficiency = DetMathf.Clamp(
    Fix64.FromRatio(workers, requiredWorkers),
    Fix64.Zero,
    Fix64.One);

// สะสมค่าทุก tick แล้วตรวจว่าครบรอบหรือยัง
// ❌ เดิม
accum += rate * deltaTime;
if (accum >= 1f) { accum -= 1f; TriggerEvent(); }

// ✅ detmath
accum = accum + rate;  // detmath ไม่มี deltaTime — simulation เดินทีละ tick คงที่
if (accum >= Fix64.One) { accum = accum - Fix64.One; TriggerEvent(); }
```

### สิ่งที่ไม่มีใน detmath (ตั้งใจ)

| ขาด | เหตุผล | ทางเลือก |
| --- | --- | --- |
| `Fix64.FromFloat(f)` | float ไม่ deterministic — รับเข้ามาไม่ได้ | ใช้ `FromRatio` หรือ `FromInt` แทน |
| `ToFloat()` / `ToDouble()` | ป้องกันนำค่ากลับไปผสมใน float logic | ใช้ `ToString()` เพื่อ display เท่านั้น |
| `Mathf.Sin` / `Cos` / `Sqrt` | ต้องการ lookup table เพื่อ determinism | ยังไม่มี — ใช้ dot product แทนในหลายกรณี |

## Determinism / ความ deterministic

Every operation uses `long` integer arithmetic only. No floating-point anywhere in simulation logic.
ทุก operation ใช้ `long` integer เท่านั้น ไม่มี floating-point ใน simulation logic

```csharp
// อย่าใช้ float หรือ double ใน simulation logic
float x = 0.1f + 0.2f;             // ผลลัพธ์อาจต่างกันบน ARM vs x86
Fix64 y = Fix64.FromRatio(3, 10);  // 0.30 — เหมือนกันทุก platform เสมอ
```

To verify cross-platform determinism, run the test suite on each target OS:
เพื่อยืนยัน determinism ข้าม platform ให้รัน test suite บนแต่ละ OS:

```bash
dotnet run --project tests/DetMath.Tests/DetMath.Tests.csproj
```

## Running Tests / รันการทดสอบ

```bash
dotnet build DetMath.sln
dotnet run --project tests/DetMath.Tests/DetMath.Tests.csproj
```

Zero external dependencies. Tests run as a plain console app.
ไม่มี dependency ภายนอก รันได้ทันทีด้วย console app ธรรมดา
