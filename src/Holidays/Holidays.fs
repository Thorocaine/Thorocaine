namespace Thorocaine

open System.Collections.Generic
open System

type Holidays (year) =
    let easter = Computus.Grogorian year

    let anual = Array.toSeq [|
            DateTime (year, 01, 01) // New Year’s Day
            DateTime (year, 03, 21) // Human Rights Day
            DateTime (year, 04, 27) // Freedom Day
            DateTime (year, 05, 01) // Workers' Day
            DateTime (year, 06, 16) // Youth Day
            DateTime (year, 08, 09) // National Women’s Day
            DateTime (year, 09, 24) // Heritage Day
            DateTime (year, 12, 16) // Day of Reconciliation
            DateTime (year, 12, 25) // Christmas Day
            DateTime (year, 12, 26) // Day of Goodwill
        |]

    let observations () =
        anual 
        |> Seq.filter (fun d -> d.DayOfWeek = DayOfWeek.Sunday)
        |> Seq.map (fun d -> d.AddDays 1.0)

    let easterHolidays () = [| -2.0 ; 1.0 |] |> Seq.map easter.AddDays

    let all () = Seq.concat [| anual ; observations() ; easterHolidays() |] |> Seq.sort
    
    let enumerator () = (all ()).GetEnumerator()

    member _.Easter with get () = easter

    interface IEnumerable<DateTime> with
        member this.GetEnumerator(): IEnumerator<DateTime> = enumerator ()
    
    interface Collections.IEnumerable with
        member this.GetEnumerator(): Collections.IEnumerator = 
            enumerator () :> _
