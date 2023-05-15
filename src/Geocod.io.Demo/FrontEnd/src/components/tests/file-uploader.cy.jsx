import FileUploader from '../file-uploader'

describe('<FileUploader />', () => {
  it('renders', () => {
    cy.mount(<FileUploader />);
    cy.get('.form-label').should('contain', 'Upload a CSV file');
    cy.get('.form-control').should('have.attr', 'type', 'file');
    cy.get('.btn').should('contain', 'Upload');

    cy.get('#file-uploaded-toast').should('not.exist');
  });

  it('Uploads a file and render returned information', () => {
    cy.intercept('POST','api/geocode/from-file', { fixture: 'good-response.json' })
    .as('geocodeFromFile');

    cy.mount(<FileUploader />);

    cy.get('#file-uploaded-toast').should('not.exist');
    cy.get('#address-cards').should('not.exist');

    cy.fixture('good-test.csv').then(fileContent => {
      cy.get('.form-control').attachFile({
        fileContent,
        filePath: 'good-test.csv'
      });

      cy.get('.btn').click();

      cy.wait('@geocodeFromFile').then((interception) => {
        expect(interception.request.body).to.contain(fileContent);
      });
    });

    cy.get('#file-uploaded-toast').should('exist');
    cy.get('#file-uploaded-toast').should('be.visible');
    cy.get('#file-uploaded-body').should('contain', 'File name: good-test.csv');

    cy.get('#address-cards').should('exist');
    cy.get('#address-cards').should('be.visible');
    cy.get(".address-card").should('have.length', 45);
  });
})
