import React from 'react'
import GeocodeTypeSelector from '../geocode-type-selector'

describe('<GeocodeTypeSelector />', () => {
  it('renders', () => {
    // see: https://on.cypress.io/mounting-react
    cy.mount(<GeocodeTypeSelector/>);

    const selector = cy.get('#geocode-type-selector');

    selector.should('contain', 'Small Batch');

    selector.click()
      .get('#large-batch-csv')
      .click()

    selector.should('contain', 'Large Batch');

  })
})
