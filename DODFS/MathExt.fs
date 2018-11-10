
module DODFS.Basic
#light

let Square x =  x*x

let Clamp value min max = 
    if (value < max) then (if(value > min) then value else min)else max

let GetBigger value1 value2 = 
    if (value1 > value2) then value1 else value2

let GetSmaller value1 value2 = 
    if (value1 < value2) then value1 else value2

let Lerp value1 value2 percent =
    (value1 * (1 - percent)) + (value2 * percent) 

let LerpSafe value1 value2 (percent:float) =
    let p = Clamp percent 0.0 1.0 
    (value1 * ((1.0) - p)) + (value2 * p) 

let LerpSafeDecimal value1 value2 (percent:decimal) =
    let p = Clamp percent 0.0m 1.0m 
    (value1 * ((1.0m) - p)) + (value2 * p) 

let InterpBy value goal amount = 
    let diff = goal - value
    Lerp value goal (if(abs(diff)>amount)then ((sign diff)* amount) else diff)
