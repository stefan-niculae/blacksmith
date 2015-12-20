# CoffeeScript

$ ->
  # Convert dates from absolute time to distance from now
  convertDates()
  # and refresh them every minute
  setInterval convertDates, 59000
  
  # Save original value for undoing 
  # and the one the db knows to save unnecessary DB accesses
  rememberInitialValues()  
  
    
convertDates = () ->
  #map `moment` format
  for elem in $(".date.difference")
    convertDate $ elem
    
convertDate = ($elem) ->
  date = $elem.attr("abs-date")
  distance = moment(date, "DD-MMM-YY HH:mm:ss").fromNow()
  $elem.text(distance) unless distance is "Invalid date"

rememberInitialValues = () ->
  for elem in $("[contenteditable]")
    $elem = $(elem)
    value = $elem.text().trim()
    $elem.data("original", value)
    $elem.data("db-cache", value)

@focused  
@registerFocus = (id, type) ->
  @focused = { id: id, type: type }
  
@updateFocused = () ->
  id = @focused.id;
  type = @focused.type
  
  if type not in ["title", "address", "description"]
    console.error "Invalid focused type: #{type}"
    return
    
  elem = $ "[db-id='#{id}']"
  field = elem.find(".#{type}")
  content = field.text().trim()
  indicator = elem.find('.working-indicator')
  
  lastContent = field.data("last-content")
  
  # No need to continue if it is at the value the db knows
  if content is field.data("db-cache")
    clearTimeout field.data("update-timeout") unless not field.data("update-timeout")
    indicator.css("visibility", "hidden")
    return
  
  # Don't continue if the keypress/deselect changed nothing
  if lastContent? and lastContent is content
    return
  field.data("last-content", content)
  
  # When updating the address, also update the favicon and the visit link
  if type is "address"
    elem.find(".favicon")
      .attr("src", "http://www.google.com/s2/favicons?domain_url=#{content}")
    elem.find(".visit-link")
      .attr("title", "Visit #{content}")
      .attr("href", "#{prependHttp content}")
      
  # Show the working indicator
  indicator.css("visibility", "visible")
  
  # Update the date
  date = elem.find(".date")
  formattedDate = moment().format("DD-MMM-YY HH:mm:ss")
  date
    .attr("abs-date", formattedDate)
    .attr("title", "Date updated: #{formattedDate}")
  
  # Give the user a 2sec period from keystroke to keystroke before
  # declaring the input finished and sending the update to the DB
  clearTimeout field.data("update-timeout") unless not field.data("update-timeout")
  f = () -> updateDB(type, content, id, indicator, date, field)
  field.data("update-timeout", setTimeout f, 2000)

@prependHttp = (link) ->
  toPrepend = "http://"
  start = link[...toPrepend.length]
  
  # Check if it doesn't already contain the http
  if start isnt toPrepend
    return toPrepend + link
  return link

@updateDB = (type, value, id, indicator, date, field) ->
  # Send DB update parameters
  $.ajax(url: "Manage?Action=Update&field=#{type}&value=#{value}&id=#{id}")
    .done ->
      # Update the new DB known value
      field.data("db-cache", value)
      # Remove the pending update indicator
      indicator.css("visibility", "hidden")
      # Update the date distance
      convertDate date
      console.log "done updating!"
      
@sendInsert = () ->
  form = $ "#creation"
  title = form.find(".title").text().trim()
  address = form.find(".address").text().trim()
  description = form.find(".description").text().trim()
  
  $.ajax(url: "Manage?Action=Insert&title=#{title}&address=#{address}&description=#{description}")
    .done ->
      console.log "done inserting!"
      
@sendDelete = (id) ->
  $.ajax(url: "Manage?Action=Delete&id=#{id}")
    .done ->
      $("[db-id='#{id}']").slideUp()
      console.log "done deleting!"