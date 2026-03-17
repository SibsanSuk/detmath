# detmath

Deterministic math library for city-building / logistics games in C#.
เป้าหมายคือทำเป็น DLL ขนาดเล็ก เอาไปใช้ใน Unity ได้ง่าย และแก้กฎ simulation ได้เร็ว
โดยหลีกเลี่ยง `float` / `double` ใน logic หลัก

## Structure

- `src/DetMath`
  deterministic primitives สำหรับ simulation
- `tests/DetMath.Tests`
  zero-dependency smoke tests รันได้ทันทีด้วย `dotnet run`

## Building Blocks

- `Fix64`
  fixed-point number แบบ 2 ทศนิยม (`Precision = 100`) — safe range ±30,370,004
- `DetVector2`
  vector 2 มิติสำหรับตำแหน่งหรือทิศทางใน simulation
- `GridPoint`
  พิกัด integer บน tile/grid
- `DetMathf`
  helper functions เช่น `Clamp`, `Lerp`, `Ratio`

## Why This Shape

- library target เป็น `netstandard2.0`
  เพื่อให้เอา DLL ไปใช้กับ Unity ได้กว้างและง่ายก่อน
- test project ใช้ console runner ธรรมดา
  เพื่อหลีกเลี่ยง dependency เพิ่ม และเริ่มเขียน test ได้ทันที
- API ตั้งต้น intentionally เล็ก
  เพื่อให้ refactor ได้คล่องตอนกฎเกมเริ่มชัดขึ้น

## Commands

```bash
dotnet build DetMath.sln
dotnet run --project tests/DetMath.Tests/DetMath.Tests.csproj
```

## Examples

### Fix64 — ตัวเลขทศนิยม deterministic

```csharp
using DetMath;

// สร้างค่าจาก int หรือ ratio
Fix64 taxRate   = Fix64.FromRatio(15, 100); // 0.15
Fix64 happiness = Fix64.FromRatio(3, 4);    // 0.75
Fix64 fullYear  = Fix64.FromInt(12);        // 12.00

// arithmetic ปกติ — ทุก platform ได้ผลเหมือนกันเสมอ
Fix64 revenue = Fix64.FromInt(5000) * taxRate;    // 750.00
Fix64 upkeep  = Fix64.FromRatio(1, 4);            // 0.25
Fix64 balance = revenue - Fix64.FromInt(200);     // 550.00
```

### Economy — ภาษีและรายได้

```csharp
// ประชากร 12,000 คน เก็บภาษีหัวละ 0.15 ต่อเดือน
Fix64 population = Fix64.FromInt(12_000);
Fix64 taxPerHead = Fix64.FromRatio(15, 100);  // 0.15
Fix64 upkeep     = Fix64.FromRatio(3, 2);     // 1.50 ต่อหัว

Fix64 income  = population * taxPerHead;  // 1800.00
Fix64 expense = population * upkeep;      // 18000.00
Fix64 deficit = income - expense;         // -16200.00
```

### Resource & Production — การผลิตทรัพยากร

```csharp
// โรงงาน 1 แห่งผลิต 2.50 หน่วยต่อรอบ มีโรงงาน 8 แห่ง
Fix64 outputPerFactory = Fix64.FromRatio(5, 2);  // 2.50
Fix64 factories        = Fix64.FromInt(8);
Fix64 totalOutput      = factories * outputPerFactory;  // 20.00

// คลังของรับได้ 75% ก่อน overflow
Fix64 storageRatio  = Fix64.FromRatio(3, 4);  // 0.75
Fix64 effectiveLoad = totalOutput * storageRatio;  // 15.00
```

### Supply Chain — ต้นทุนขนส่ง

```csharp
// ระยะทาง 14 tile ความเร็ว 1.50 tile/tick → ใช้กี่ tick?
Fix64 speed    = Fix64.FromRatio(3, 2);   // 1.50
Fix64 distance = Fix64.FromInt(14);
Fix64 ticks    = distance / speed;        // 9.33

// clamp happiness ไม่ให้เกิน 1 และไม่ต่ำกว่า 0
Fix64 raw       = Fix64.FromRatio(13, 10);  // 1.30 (เกิน)
Fix64 clamped   = DetMathf.Clamp(raw, Fix64.Zero, Fix64.One);  // 1.00

// interpolate ระหว่างสองค่า
Fix64 baseSpeed   = Fix64.FromInt(2);
Fix64 boostSpeed  = Fix64.FromInt(5);
Fix64 t           = Fix64.FromRatio(1, 3);
Fix64 actualSpeed = DetMathf.Lerp(baseSpeed, boostSpeed, t);  // 3.00
```

### GridPoint — ตำแหน่ง tile และระยะทาง

```csharp
// ตำแหน่งของอาคารบน grid
GridPoint warehouse = new(3, 7);
GridPoint market    = new(8, 2);

// Manhattan distance สำหรับคิด path cost บน grid
int tiles = warehouse.ManhattanDistance(market);  // 10

// เลื่อนตำแหน่ง
GridPoint offset = new(1, 0);
GridPoint next   = warehouse + offset;  // (4, 7)
```

### DetVector2 — ทิศทางและ influence

```csharp
// คำนวณ dot product เพื่อดูว่า unit หันไปทางศัตรูแค่ไหน
DetVector2 facing  = new(Fix64.One, Fix64.Zero);   // หันขวา
DetVector2 toEnemy = new(Fix64.FromRatio(4, 5), Fix64.FromRatio(3, 5));

Fix64 alignment = DetVector2.Dot(facing, toEnemy);  // 0.80

// scale velocity ด้วย speed multiplier
DetVector2 velocity = new(Fix64.FromInt(3), Fix64.FromInt(4));
Fix64      slow     = Fix64.FromRatio(1, 2);
DetVector2 slowed   = velocity * slow;  // (1.50, 2.00)
```

### ToIntFloor vs ToIntTruncate

```csharp
// ใช้ ToIntFloor เมื่อต้องการ "จำนวนรอบเต็มที่ผ่านไปแล้ว"
Fix64 elapsed = Fix64.FromRatio(7, 2);    // 3.50
int completedCycles = elapsed.ToIntFloor();   // 3  ✓

// ระวัง: ค่าลบ floor vs truncate ต่างกัน
Fix64 debt = Fix64.FromRaw(-150);   // -1.50
debt.ToIntFloor();    // -2  (floor ลงไป)
debt.ToIntTruncate(); // -1  (ตัดทิ้งทศนิยม)
```

### สิ่งที่ไม่ควรทำ

```csharp
// อย่าผสม float/double กับ Fix64 ใน simulation logic
// float และ double ไม่ deterministic ข้าม platform

float bad = 0.1f + 0.2f;               // อาจได้ผลต่างกันบน ARM vs x86
Fix64 good = Fix64.FromRatio(3, 10);   // 0.30 ทุก platform เสมอ
```

## Next Good Steps

1. เพิ่ม `Fix64.Sqrt` หรือ lookup-based trig ถ้าจะใช้ movement เชิงต่อเนื่อง
2. แยก namespace สำหรับ economy / logistics / path cost
3. เพิ่ม serialization helpers ถ้าจะ save/load raw values ตรง ๆ
4. เพิ่ม GitHub Actions matrix (ubuntu / windows / macos) เพื่อพิสูจน์ determinism ข้าม platform
