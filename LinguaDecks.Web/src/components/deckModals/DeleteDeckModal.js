import { Modal, Button } from "react-bootstrap";
import React from 'react'

function DeleteDeckModal({ show, onHide, onConfirm }) {
  return (
    <Modal show={show} onHide={onHide}>
      <Modal.Header closeButton>
        <Modal.Title>Delete deck</Modal.Title>
      </Modal.Header>
      <Modal.Body>Are you sure you want to delete this deck?<br/>This action cannot be undone.</Modal.Body>
      <Modal.Footer>
        <Button variant="primary" onClick={onConfirm}>
          Yes
        </Button>
        <Button variant="secondary" onClick={onHide}>
          No
        </Button>
      </Modal.Footer>
    </Modal>
  );
}

export default DeleteDeckModal