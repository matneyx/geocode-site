import Container from 'react-bootstrap/Container';
import Navbar from 'react-bootstrap/Navbar';

const SiteNavbar = () => (
  <Navbar bg="dark" variant="dark" expand="lg">
    <Container>
      <Navbar.Brand href="/" id='title'>Geocod.io Demo</Navbar.Brand>
    </Container>
  </Navbar>
)

export default SiteNavbar;
