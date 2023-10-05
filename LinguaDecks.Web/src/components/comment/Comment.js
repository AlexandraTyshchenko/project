import axios from 'axios';
import React, { useState } from 'react';

const api_URL = process.env.REACT_APP_API_URL;

function Comment(props) {
  const [comment, SetComment] = useState("");
  const [error, SetError] = useState("");

  const handleCommentChange = (event) => {
    SetComment(event.target.value);
  };

  const CreateComment = async () => {
    try {
      const url = `${api_URL}/api/Comments?userID=${props.userId}&dekcId=${props.deckId}&comment=${comment}`;
      await axios.post(url);
      props.SetUpdate();
      SetComment("");
    } catch (errormessage) {
      if (errormessage.response) {
        SetError(errormessage.response.data.Details);
      } else {
        SetError(errormessage.message);
      }
    }
  };

  const handleSubmit = (event) => {
    event.preventDefault(); // Отменить стандартное поведение формы (перезагрузку страницы)
    CreateComment(); // Вызвать функцию CreateComment при отправке формы
  };

  return (
    <form onSubmit={handleSubmit}>
      <input
        className="form-control form-control-lg"
        type="text"
        placeholder="Write a comment"
        aria-label=".form-control-lg example"
        value={comment}
        onChange={handleCommentChange}
      />
      <button type="submit" className="btn btn-secondary mt-2 mb-4">Publish</button>
      {error ? (
        <div className="alert alert-danger text-center" role="alert">{error}</div>
      ) : null}
    </form>
  );
}

export default Comment;
