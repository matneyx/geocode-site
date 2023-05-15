Feature: Geocod_io_Demo
  As a user
  I want to use the geocod.io API
  So that I can get the latitude and longitude of a list of given addresses

   Scenario: I can upload a file
    Given I have loaded the site
    And There is a file upload field
    When I select a file to upload
    And I click the Upload button
    Then I should get a message that the file was uploaded
    And I should get a list of coordinates for the addresses in the file
    
   # TODO: Add scenario for invalid file
