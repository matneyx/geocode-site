import SiteNavbar from '../site-navbar'

describe('<SiteNavbar />', () => {
  it('renders', () => {
    cy.mount(<SiteNavbar />);
    cy.get('#title').should('contain', 'Geocod.io Demo');
  })
})
