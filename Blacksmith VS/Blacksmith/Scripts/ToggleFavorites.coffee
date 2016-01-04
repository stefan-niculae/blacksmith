# CoffeeScript
  
@sendToggle = (address) ->
  elem = $ event.currentTarget 
  $.ajax url: "Link?addr=#{address}&toggle-fav"
    .done ->
      console.log "done toggling fav"
      visuallyToggle elem
        
visuallyToggle = (elem) ->
  
  indicator = elem.find ".fav-indicator"
  count = elem.find ".fav-count"
  isFavorited = indicator.hasClass "star"

  console.log "is fav = #{isFavorited}, count = #{count.text()}"
  if isFavorited
    indicator.removeClass "star"
    indicator.addClass "no-star"
    count.text(+count.text() - 1)
  else
    indicator.addClass "star"
    indicator.removeClass "no-star"
    count.text(+count.text() + 1)