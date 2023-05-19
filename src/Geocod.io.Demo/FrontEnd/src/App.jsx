import GeocodeTypeSelector from "./components/geocode-type-selector.jsx";
import SiteNavbar from './components/site-navbar';
import Stack from 'react-bootstrap/Stack';

'./components/site-navbar';

const App = () => {

  return (
    <>
      <SiteNavbar/>
      <Stack gap={2} className="col-md-5 mx-auto">
        <GeocodeTypeSelector />
      </Stack>
    </>
  )
};

export default App
