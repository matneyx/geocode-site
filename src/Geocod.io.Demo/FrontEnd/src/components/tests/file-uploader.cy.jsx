import FileUploader from '../file-uploader'

describe('<FileUploader />', () => {
  it('renders', () => {
    cy.mount(<FileUploader/>);
    cy.get('.form-control').should('have.attr', 'type', 'file');
    cy.get('.btn').should('contain', 'Upload');
    cy.get('.btn').should('be.disabled');

    cy.get('#file-uploaded-toast').should('not.exist');
  });

  describe('Small Batch', () => {
    it('Uploads a file and render returned information', () => {
      cy.intercept('POST', 'api/geocode/from-file', {fixture: 'good-response.json'})
        .as('geocodeFromFile');

      cy.mount(<FileUploader selectedOptionId="small-batch"/>);

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
  });

  describe('Large Batch', () => {
    it('Uploads a file and render returned information', () => {
      cy.mount(<FileUploader selectedOptionId="large-batch"/>);

      cy.get('#upload-progress').should('not.exist');
      cy.get('#file-uploaded-toast').should('not.exist');
      cy.get('#address-cards').should('not.exist');

      cy.fixture('good-test.csv').then(fileContent => {
        cy.get('.form-control').attachFile({
          fileContent,
          filePath: 'good-test.csv'
        });
      });

      cy.window().then((win) => {
        const data = win["cypress-signalr-mock"];
        expect(data.mocks[1]).to.be.undefined;
      });

      cy.get('.btn').click();

      cy.window().then((win) => {
        const data = win["cypress-signalr-mock"];
        expect(data.mocks[1]._serverInvokes).to.have.length(1);
        expect(data.mocks[1]._serverInvokes[0].action).to.equal('SendHandshake');
        expect(data.mocks[1]._serverInvokes[0].args[0].connected).to.be.true;
      });

      cy.hubPublish(
        '/hubs/geocode',
        'GeocodeStart',
        {"connected": true}
      );

      cy.get("#upload-progress").should('exist');
      cy.get("#upload-progress").should('be.visible');
      cy.get(".progress-bar").invoke('attr', 'aria-valuenow').should('equal', '0');
      cy.get(".progress-bar").should('contain', '0%');

      cy.window().then((win) => {
        const data = win["cypress-signalr-mock"];
        expect(data.mocks[1]._serverInvokes).to.have.length(2);
        expect(data.mocks[1]._serverInvokes[1].action).to.equal('UploadFile');
        expect(data.mocks[1]._serverInvokes[1].args[0]).to.be.a('FormData');
      });

      cy.hubPublish(
        '/hubs/geocode',
        'GeocodeUpdate',
        {progress: 20}
      );

      cy.get(".progress-bar").invoke('attr', 'aria-valuenow').should('equal', '20');
      cy.get(".progress-bar").should('contain', '20%');

      cy.fixture('good-response.json').then(response => {
        cy.hubPublish(
          '/hubs/geocode',
          'GeocodeComplete',
          response
        );
      });

      cy.get('#address-cards').should('exist');
      cy.get('#address-cards').should('be.visible');
      cy.get(".address-card").should('have.length', 45);
    });
  });
});
