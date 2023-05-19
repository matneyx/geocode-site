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
      cy.intercept('POST', 'api/geocode/small-batch', {fixture: 'good-response.json'})
        .as('geocodeSmallBatch');

      cy.mount(<FileUploader selectedOptionId="small-batch"/>);

      cy.get('#file-uploaded-toast').should('not.exist');
      cy.get('#address-cards').should('not.exist');

      cy.fixture('good-test.csv').then(fileContent => {
        cy.get('.form-control').attachFile({
          fileContent,
          filePath: 'good-test.csv'
        });

        cy.get('.btn').click();

        cy.wait('@geocodeSmallBatch').then((interception) => {
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
      cy.intercept('POST', 'api/geocode/large-batch', {statusCode: 202, body:{ batchId: 8675309}})
        .as('geocodeLargeBatch');

      cy.fixture('good-response.json').then(fileContent => {
        cy.intercept('GET', 'api/geocode/download-results?batchId=8675309', { body: fileContent })
          .as('downloadResults');
      });

      cy.mount(<FileUploader selectedOptionId="large-batch"/>);

      cy.get('#upload-progress').should('not.exist');
      cy.get('#file-uploaded-toast').should('not.exist');
      cy.get('#address-cards').should('not.exist');

      cy.fixture('good-test.csv').then(fileContent => {
        cy.get('.form-control').attachFile({
          fileContent,
          filePath: 'good-test.csv'
        });

        cy.window().then((win) => {
          const data = win["cypress-signalr-mock"];
          expect(data.mocks[1]).to.be.undefined;
        });

        cy.get('.btn').click();

        cy.wait('@geocodeLargeBatch').then((interception) => {
          expect(interception.request.body).to.contain(fileContent);
        });
      });

      cy.window().then((win) => {
        const data = win["cypress-signalr-mock"];
        expect(data.mocks[1]._serverInvokes).to.have.length(1);
        expect(data.mocks[1]._serverInvokes[0].action).to.equal('SendHandshake');
        expect(data.mocks[1]._serverInvokes[0].args[0].batchId).to.equal(8675309);
      });

      cy.hubPublish(
        '/hubs/geocode',
        'GeocodeStart',
        { batchId: 8675309, progress: 0 }
      );

      cy.get("#upload-progress").should('exist');
      cy.get("#upload-progress").should('be.visible');
      cy.get(".progress-bar").invoke('attr', 'aria-valuenow').should('equal', '0');
      cy.get(".progress-bar").should('contain', '0%');

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
          {progress: 100}
        );

        cy.get(".progress-bar").invoke('attr', 'aria-valuenow').should('equal', '100');
        cy.get(".progress-bar").should('contain', 'Upload Complete');

        cy.wait('@downloadResults');

        cy.get('#upload-progress').should('not.exist');
        cy.get('#file-uploaded-toast').should('not.exist');
        cy.get('#address-cards').should('exist');
        cy.get('#address-cards').should('be.visible');
        cy.get(".address-card").should('have.length', 45);
      });
    });
  });
});
