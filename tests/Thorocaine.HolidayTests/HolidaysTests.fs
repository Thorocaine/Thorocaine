module Thorocaine.HolidaysTests

open System 
open NUnit.Framework
open Thorocaine

[<SetUp>]
let Setup () =
    ()

let YearAndHolidays = [| 
    [| 2020:>obj; 13:>obj |]
    [| 2021:>obj; 14:>obj |]
    [| 2022:>obj; 14:>obj |]
|]

[<TestCaseSource("YearAndHolidays")>]
let ``Has one`` year holidays =
    let result = Holidays(year) |> Seq.length
    Assert.AreEqual (holidays, result)
