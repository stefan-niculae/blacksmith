# CoffeeScript

@submitSimilar = () ->
  selected = $("#new-similar select").val()
  $.get "#{window.location.href}&new-similar=#{selected}", (data) ->
    newSimilars = $(data).find("#submitted-similars").children()
    $("#submitted-similars").replaceWith newSimilars
    deleteOption selected
    console.log "done submitting similar"
    
deleteOption = (address) ->
  elem = $ "#similars option:contains('#{address}')"
  elem.remove()
    
@deleteSimilar = () ->
  target = $ event.currentTarget
  address = target.siblings(".address").text()
  elem = target.parent(".similar-link")
  $.get "#{window.location.href}&del-similar=#{address}", ->
    elem.slideUp("fast")
    addOption address
    console.log "done deleting similar"
    
addOption = (address) ->
  list = $ "#similars select"
  list.append "<option>#{address}</option>"