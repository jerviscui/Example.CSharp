using StructTest;

var m1 = new Measurement();
Console.WriteLine(m1);  // output: NaN (Undefined)

var m2 = default(Measurement);
Console.WriteLine(m2);  // output: 0 ()

var ms = new Measurement[2];
Console.WriteLine(string.Join(", ", ms));  // output: 0 (), 0 ()

ValueTypeTest.ShowSize();

ValueTypeTest.OutOfPrecisionTrueTest();
ValueTypeTest.OutOfPrecisionFalseTest();
