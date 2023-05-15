import { Card, Container, Row, Col } from 'react-bootstrap';
import PropTypes from 'prop-types';
import styled from 'styled-components';

  // "accuracy": 1,
  // "accuracyType": "rooftop",
  // "formattedAddress": "660 Pennsylvania Ave SE, Washington, DC 20003",
  // "latitude": 38.885172,
  // "longitude": -76.996565

var AddressRow = styled(Row)`
  font-weight: bold;
  `;

var SectionHeader = styled.span`
  font-weight: 600;
`;

var AccuracyType = styled.span`
  font-style: italic;
`;

const AddressCard = ({formattedAddress, latitude, longitude, accuracy, accuracyType}) => (
<Card className="address-card">
  <Card.Body>
    <Container>
      <AddressRow id="formatted-address">
      {formattedAddress}
      </AddressRow>
      <Row id="coordinates">
        <Col id="latitude"><SectionHeader id="latitude-header">Latitude:</SectionHeader> {latitude}</Col>
        <Col id="longitude"><SectionHeader id="longitude-header">Longitude:</SectionHeader> {longitude}</Col>
        </Row>
      <Row>
        <Col id="accuracy"><SectionHeader id="accuracy-header">Accuracy:</SectionHeader> {accuracy} (<AccuracyType id="accuracy-type">{accuracyType}</AccuracyType>)</Col>
      </Row>
    </Container>
  </Card.Body>
</Card>);

export default AddressCard;

AddressCard.propTypes = {
  formattedAddress: PropTypes.string.isRequired,
  latitude: PropTypes.number.isRequired,
  longitude: PropTypes.number.isRequired,
  accuracy: PropTypes.number.isRequired,
  accuracyType: PropTypes.string.isRequired
};
