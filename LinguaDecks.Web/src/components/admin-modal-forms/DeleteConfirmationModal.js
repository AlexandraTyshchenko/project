import React from "react";
import { Modal, Button } from "react-bootstrap";

function DeleteConfirmationModal({ show, onHide, onConfirmDelete }) {
  return (
    <Modal show={show} onHide={onHide}>
      <Modal.Header closeButton>
        <Modal.Title>Delete Category</Modal.Title>
      </Modal.Header>
      <Modal.Body>Are you sure you want to delete this category?</Modal.Body>
      <Modal.Footer>
       
        <Button variant="danger" onClick={onConfirmDelete}>
          Yes
        </Button>
        <Button variant="secondary" onClick={onHide}>
          No
        </Button>
      </Modal.Footer>
    </Modal>
  );
}

export default DeleteConfirmationModal;
