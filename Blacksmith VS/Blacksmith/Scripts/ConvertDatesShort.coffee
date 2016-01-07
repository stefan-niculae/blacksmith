# CoffeeScript

$ ->
  convertDatesShort()
  
convertDatesShort = () ->
  for elem in $ ".date.short-diff"
    $elem = $ elem
    original = $elem.text()
    
    distance = moment(original, "DD-MMM-YY HH:mm:ss").fromNow(true)
    return if distance is "Invalid Date"
    
    shortened = distance
      .replace /\s*a few seconds\s*/i, "now"
      .replace /\s*seconds?\s*/i, "s"
      .replace /\s*minutes?\s*/i, "m"
      .replace /\s*hours?\s*/i,   "h"
      .replace /\s*days?\s*/i,    "d"
      .replace /\s*months?\s*/i,  "mo"
      .replace /\s*years?\s*/i,   "y"
      .replace /\s*a\s*/i,        "1"
      
    $elem.text(shortened)