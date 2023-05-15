import FileUploader from './components/file-uploader';
import SiteNavbar from './components/site-navbar';
import Stack from 'react-bootstrap/Stack';

'./components/site-navbar';

const App = () => {

  return (
    <>
      <SiteNavbar/>
      <Stack gap={2} className="col-md-5 mx-auto">
        <FileUploader/>
      </Stack>
    </>
  )
};

export default App
