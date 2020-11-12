module Thorocaine.EasterTests

open System 
open NUnit.Framework
open Thorocaine

[<SetUp>]
let Setup () =
    ()

[<DatapointSource>]
let easters = [| 
    DateTime(2016, 03, 27)
    DateTime(2019, 04, 21)
    DateTime(2020, 04, 12)
    DateTime(2021, 04, 04)
    DateTime(2024, 03, 31)
|]

[<DatapointSource>]
let years = seq { 1900 .. 2200 }

[<Theory>]
let ``Easter is correct`` (easter: DateTime) =
    let result = Holidays(easter.Year).Easter
    Assert.AreEqual (easter, result)

[<Theory>]
let ``Easter is always on a sunday`` (year: int) =
    let result = Holidays(year).Easter
    Assert.AreEqual(DayOfWeek.Sunday , result.DayOfWeek)