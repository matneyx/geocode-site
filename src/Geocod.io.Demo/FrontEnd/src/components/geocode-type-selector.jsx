import {useState} from "react";
import {Col, Container, Dropdown, Row} from "react-bootstrap";
import FileUploader from "./file-uploader";

export const dropdownOptions = [
  {
    id: "single-address",
    displayText: "Single Address"
  },
  {
    id: "batch-csv",
    displayText: "Small Batch"
  },
  {
    id: "large-batch-csv",
    displayText: "Large Batch"
  }
];
const GeocodeTypeSelector = () => {
  const [selectedOption, setSelectedOption] = useState(dropdownOptions[0]);

  return (
    <Container>
      <Row>
        <Col xs={3}>
          <Dropdown className="d-inline-block">
            <Dropdown.Toggle className="w-100" id="geocode-type-selector">
              {selectedOption.displayText}
            </Dropdown.Toggle>

            <Dropdown.Menu>
              {
                dropdownOptions.map((option) => (
                  <Dropdown.Item id={option.id} onClick={() => setSelectedOption(option)}>
                    {option.displayText}
                  </Dropdown.Item>))
              }
            </Dropdown.Menu>
          </Dropdown>
        </Col>
        <Col>
          <FileUploader
            selectedOptionId={selectedOption.id}/>
        </Col>
      </Row>
    </Container>
  );
};

export default GeocodeTypeSelector;
