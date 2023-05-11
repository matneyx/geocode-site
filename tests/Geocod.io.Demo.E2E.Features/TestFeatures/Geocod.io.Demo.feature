Feature: Geocod_io_Demo
  As a user
  I want to use the geocod.io API
  So that I can get the latitude and longitude of a list of given addresses

  Scenario: I can navigate to the site
    Given I have a web browser
    When I navigate to the site
    Then I should see the site title
