module Computus

open System

let Grogorian year =
    let a = year % 19
    let b = year % 4
    let c = year % 7
    let k = year / 100
    let p = (8*k + 13) / 25
    let q = k / 4
    let M = (15 - p + k - q) % 30
    let N = (4 + k - q) % 7
    let d = (19*a + M) % 30
    let e = (2*b + 4*c + 6*d + N) % 7
    
    match (22 + d + e) with
    | x when x <= 31 -> DateTime (year, 3, x)
    | _ -> match (d, e) with
           | (29, 6) -> DateTime (year, 4, 19)
           | (28, 6) when ((11*M + 11) % 30) < 19 -> DateTime (year, 4, 18)
           | _ -> DateTime (year, 4, d + e - 9)