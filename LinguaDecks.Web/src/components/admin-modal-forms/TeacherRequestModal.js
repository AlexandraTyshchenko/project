import React from "react";
import { Modal, Button } from "react-bootstrap";

function TeacherRequestModal({ show, onHide, title, message, onConfirm }) {
  return (
    <Modal show={show} onHide={onHide} backdrop="static">
      <Modal.Header closeButton>
        <Modal.Title>{title}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <p>{message}</p>
      </Modal.Body>
      <Modal.Footer>
        <button className="btn btn-success" onClick={() => onConfirm(true)}>
          Yes
        </button>
        <button className="btn btn-danger" onClick={() => onConfirm(false)}>
          No
        </button>
      </Modal.Footer>
    </Modal>
  );
}

export default TeacherRequestModal;
