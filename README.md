# detmath

Deterministic math library for city-building / logistics games in C#.
เป้าหมายคือทำเป็น DLL ขนาดเล็ก เอาไปใช้ใน Unity ได้ง่าย และแก้กฎ simulation ได้เร็ว
โดยหลีกเลี่ยง `float` / `double` ใน logic หลัก

## Structure

- `src/DetMath`
  deterministic primitives สำหรับ simulation
- `tests/DetMath.Tests`
  zero-dependency smoke tests รันได้ทันทีด้วย `dotnet run`

## Current Building Blocks

- `Fix64`
  fixed-point number แบบ 3 ตำแหน่งทศนิยม (`Precision = 1000`)
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

## Example

```csharp
using DetMath;

Fix64 roadSpeed = Fix64.FromRatio(3, 2); // 1.500
Fix64 travelTime = Fix64.FromInt(12) / roadSpeed;

DetVector2 offset = new(Fix64.FromInt(4), Fix64.FromInt(2));
GridPoint house = new(10, 6);
```

## Next Good Steps

1. เพิ่ม `Fix64.Sqrt` หรือ lookup-based trig ถ้าจะใช้ movement เชิงต่อเนื่อง
2. แยก namespace สำหรับ economy / logistics / path cost
3. เพิ่ม serialization helpers ถ้าจะ save/load raw values ตรง ๆ
