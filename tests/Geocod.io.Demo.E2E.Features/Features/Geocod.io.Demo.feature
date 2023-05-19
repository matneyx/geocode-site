Feature: Geocod_io_Demo
  As a user
  I want to use the geocod.io API
  So that I can get the latitude and longitude of a list of given addresses

   Scenario: I can Geocode a small batch of addresses from a file
    Given I have loaded the site
    And I have selected "Small Batch" from the menu
    And There is a file upload field
    When I select a file to upload
    And I click the Upload button
    Then I should get a message that the file was uploaded
    And I should get a list of 24 coordinates for the addresses in the file

   # TODO: Add scenario for invalid file

    Scenario: I can Geocode a large batch of addresses from a file
      Given I have loaded the site
      And I have selected "Large Batch" from the menu
      And There is a file upload field
      When I select a file to upload
      And I click the Upload button
      # TODO: Verify progress bar
      # TODO: Figure out why I'm getting a different number of coordinates from the same file
      Then I should get a list of 23 coordinates for the addresses in the file
