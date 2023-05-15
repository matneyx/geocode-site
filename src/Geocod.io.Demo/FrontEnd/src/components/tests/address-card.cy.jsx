import AddressCard from '../address-card'

const defaultProps = {
  "accuracy": 1,
  "accuracyType": "rooftop",
  "formattedAddress": "660 Pennsylvania Ave SE, Washington, DC 20003",
  "latitude": 38.885172,
  "longitude": -76.996565
};

describe('<AddressCard />', () => {
  it('renders', () => {
    cy.mount(<AddressCard {...defaultProps}/>);
    cy.get('#formatted-address').should('exist');
    cy.get('#formatted-address').should('be.visible');
    cy.get('#formatted-address').should('contain', defaultProps.formattedAddress);
    cy.get('#formatted-address').should('have.css', 'font-weight', '700'); // bold

    cy.get('#coordinates').should('exist');
    cy.get('#coordinates').should('be.visible');

    cy.get('#latitude').should('exist');
    cy.get('#latitude').should('be.visible');
    cy.get('#latitude').should('contain', defaultProps.latitude);
    cy.get('#latitude-header').should('have.css', 'font-weight', '600'); // semi-bold

    cy.get('#longitude').should('exist');
    cy.get('#longitude').should('be.visible');
    cy.get('#longitude').should('contain', defaultProps.longitude);
    cy.get('#longitude-header').should('have.css', 'font-weight', '600'); // semi-bold

    cy.get('#accuracy').should('exist');
    cy.get('#accuracy').should('be.visible');
    cy.get('#accuracy').should('contain', `Accuracy: ${defaultProps.accuracy} (${defaultProps.accuracyType}`);
    cy.get('#accuracy-header').should('have.css', 'font-weight', '600'); // semi-bold
    cy.get('#accuracy-type').should('have.css', 'font-style', 'italic'); // italic

  })
})
