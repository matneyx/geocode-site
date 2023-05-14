import {useRef, useState} from "react";
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import Toast from 'react-bootstrap/Toast';
import InputGroup from 'react-bootstrap/InputGroup';

const FileUploader = () => {
  const [showToast, setShowToast] = useState(false);
  const [disableButton, setDisableButton] = useState(true);
  const [uploadedFileName, setUploadedFileName] = useState(null);
  const inputRef = useRef(null);

  const handleUpload = async () => {
    if (inputRef.current?.files) {

      const data = new FormData();
      data.append('file', inputRef.current?.files[0]);

      await fetch('/api/geocode/from-file', {
        method: 'POST',
        body: data
      }).then(response => {
        setUploadedFileName(inputRef.current.files[0].name);
        setShowToast(true);
      });
    }
  };

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
    <>
      <Form>
        <Form.Label>Upload a CSV file</Form.Label>
        <InputGroup className="mb-3" id="upload-section">
          <Form.Control
            type="file"
            id="upload-input"
            ref={inputRef}
            onChange={handleFileChange}
          />
          <Button
            disabled={disableButton}
            variant={uploadedFileName ? 'success' : 'primary'}
            id="upload-button"
            onClick={(handleUpload)}>
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
    </>
  );
};

export default FileUploader;
