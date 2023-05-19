import {useEffect, useRef, useState} from "react";
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import Toast from 'react-bootstrap/Toast';
import InputGroup from 'react-bootstrap/InputGroup';
import ProgressBar from 'react-bootstrap/ProgressBar';
import {Col, Container, Row} from "react-bootstrap";
import AddressCard from "./address-card";
import {HubConnectionBuilder} from "@microsoft/signalr";
import {useCypressSignalRMock} from "cypress-signalr-mock";

const FileUploader = ({selectedOptionId}) => {
  const [showToast, setShowToast] = useState(false);
  const [showProgressBar, setShowProgressBar] = useState(false);
  const [uploadProgress, setUploadProgress] = useState(0);
  const [batchId, setBatchId] = useState(null);
  const [uploadStatus, setUploadStatus] = useState(false);
  const [connection, setConnection] = useState(null);
  const [disableButton, setDisableButton] = useState(true);
  const [uploadedFileName, setUploadedFileName] = useState(null);
  const [addressCards, setAddressCards] = useState([]);
  const [showAddressCards, setShowAddressCards] = useState(false);
  const inputRef = useRef(null);

  useEffect(() => {
    if (connection && batchId) {
      connection.start()
        .then(async () => {
          connection.on("GeocodeStart", (message) => {
            console.log("GeocodeStart");
            setUploadProgress(message.progress);
            setUploadStatus(`${message.progress}%`);
            setShowProgressBar(true);
          });

          connection.on("GeocodeUpdate", (message) => {
            setUploadProgress(message.progress);
            setUploadStatus(`${message.progress}%`);
          });

          connection.on("GeocodeComplete", async () => {
            console.log("GeocodeComplete")
            setUploadProgress(100);
            setUploadStatus("Upload Complete");

            await downloadResults();

            connection.stop();
          });

          await connection.invoke("SendHandshake", {batchId: batchId});
        })
        .catch(e => console.log('Connection failed: ', e));
    }
  }, [connection, batchId]);

  const handleSingleAddressUpload = async () => {
  };

  const handleSmallBatchUpload = async () => {
    if (inputRef.current?.files) {
      const data = new FormData();
      data.append('file', inputRef.current?.files[0]);

      await fetch('/api/geocode/small-batch', {
        method: 'POST',
        body: data
      }).then(async response => {
        console.log("Small Batch:")
        const responseBody = await response.json();
        setShowAddressCards(true);
        setAddressCards(responseBody);
        setUploadedFileName(inputRef.current.files[0].name);
        setShowToast(true);
      });
    }
  };

  const handleLargeBatchUpload = async () => {
    console.log("handleLargeBatchUpload")
    if (inputRef.current?.files) {
      const data = new FormData();
      data.append('file', inputRef.current?.files[0]);

      await fetch('/api/geocode/large-batch', {
        method: 'POST',
        body: data
      }).then(async response => {
        const responseBody = await response.json();

        await initiateSignalRConnection()
          .then(() => {
            console.log("Connection started");
            setBatchId(responseBody.batchId);
          });
      });
    }
  };
  const initiateSignalRConnection = async () => {
    console.log("initiateSignalRConnection")
    const newConnection = useCypressSignalRMock('/hubs/geocode') ??
      new HubConnectionBuilder()
        .withUrl("http://localhost:5216/hubs/geocode")
        .withAutomaticReconnect()
        .build();

    setConnection(newConnection);
  }

  const downloadResults = async () => {
    console.log("downloadResults")
    await fetch('/api/geocode/download-results?' + new URLSearchParams({ batchId }), {
      method: 'GET'
    }).then(async response => {
      const responseBody = await response.json();
      console.log(responseBody);
      setShowProgressBar(false);
      setShowAddressCards(true);
      setAddressCards(responseBody);
    }).catch(e => console.log(`Something went wrong: ${e}`));
  };

  const handleUpload = {
    "single-address": () => handleSingleAddressUpload(),
    "small-batch": () => handleSmallBatchUpload(),
    "large-batch": () => handleLargeBatchUpload(),
  }

  const handleFileChange = () => {
    setDisableButton(Boolean(inputRef.current?.files) === false);
  }

  const toggleToast = () => {
    setShowToast(!showToast);
    setUploadedFileName(null);
    setDisableButton(true);
    inputRef.current.value = null;
  }

  return (
    <Container>
      <Row>
        <Col>
          <Form>
            <InputGroup className="mb-3" id="upload-section">
              <Form.Control
                type="file"
                id="upload-input"
                ref={inputRef}
                onChange={handleFileChange}
              />
              <Button
                disabled={disableButton}
                variant={disableButton ? 'secondary' : uploadedFileName ? 'success' : 'primary'}
                id="upload-button"
                onClick={() => handleUpload[selectedOptionId]()}>
                {uploadedFileName ? "Success!" : "Upload"}
              </Button>
            </InputGroup>
          </Form>
          <Toast id="file-uploaded-toast" show={showToast} onClick={toggleToast}>
            <Toast.Header>
              <strong className="me-auto">File Uploaded</strong>
            </Toast.Header>
            <Toast.Body id="file-uploaded-body">File name: {uploadedFileName}</Toast.Body>
          </Toast>
        </Col>
      </Row>
      {showProgressBar && <Row>
        <Col>
          <ProgressBar id="upload-progress" now={uploadProgress} label={uploadStatus}/>
        </Col>
      </Row>}
      {showAddressCards &&
        <Row>
          <Col>
            <div id="address-cards">
              {addressCards.map((addressCardInformation, index) => <AddressCard
                key={index} {...addressCardInformation} />)}
            </div>
          </Col>
        </Row>
      }
    </Container>
  );
};

export default FileUploader;
