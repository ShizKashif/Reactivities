import React, { Fragment } from "react";
import { Link } from "react-router-dom";
import { Container } from "semantic-ui-react";

export const HomePage = () => {
  return (
    <Fragment>
      <Container style={{ marginTop: "7em" }}>
        <h1>Home page</h1>
        <h3>Go to <Link to='/activities'>Activities</Link></h3>
      </Container>
    </Fragment>
  );
};

export default HomePage;
