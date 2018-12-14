import Rect, { Component } from 'react';
import { Glyphicon, Nav, Navbar, NavItem } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import { Link } from 'react-router-dom';

export default class NavBar extends Component {
    render() {
        return (
            <Navbar inverse fixedTop fluid collapseOnSelect>
                <Navbar.Header>
                    <Navbar.Brand>
                        {/*<Link to={'/'}>Home</Link>*/}
                    </Navbar.Brand>
                    <Navbar.Toggle />
                </Navbar.Header>
                <Navbar.Collapse> 
                    <Nav>
                        <LinkContainer to={'/MyCounter'}>
                            <NavItem>
                                <Glyphicon glyph='home' /> MyCounter
                            </NavItem>
                        </LinkContainer>
                        <LinkContainer to={'/'} exact>
                            <NavItem>
                                <Glyphicon glyph='home' /> Home
                            </NavItem>
                        </LinkContainer>
                        <LinkContainer to={'/counter'}>
                            <NavItem>
                                <Glyphicon glyph='education' /> Counter
                            </NavItem>
                        </LinkContainer>
                        <LinkContainer to={'/fetchdata'}>
                            <NavItem>
                                <Glyphicon glyph='th-list' /> Fetch data
                            </NavItem>
                        </LinkContainer>
                    </Nav>
                </Navbar.Collapse>
            </Navbar>
        );
    }
}