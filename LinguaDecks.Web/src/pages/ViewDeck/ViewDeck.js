import './ViewDeck.css';
import { Link } from 'react-router-dom';
import axios from 'axios';
import React, { useState } from 'react';
import deckIcon from '../../assets/default_deck_logo.svg'
import ProgressBar from '../../components/DeckCards/ProgressBar';
import DeleteDeckModal from '../../components/deckModals/DeleteDeckModal';
import Comment from '../../components/comment/Comment';

function DeckPage(props) {
  const api_URL = process.env.REACT_APP_API_URL;

  var [deck, setDeck] = useState(props.Deck);
  const [modalVisible, setModalVisible] = useState(false);
  const progress = props.UserProgress;
  const deckRating = props.DeckRating;
  const user = props.User;
  const comments = props.Comments;
  const [error, setError] = useState(""); 

  async function deleteCard(cardId) {
    try {
      await axios.delete(`${api_URL}/api/cards/${cardId}`);
    } catch (error) {
      console.error("Error deleting data: ", error);
    }
    await updateCardsData();
  }

  async function updateCardsData() {
    try {
      const response = await axios.get(`${api_URL}/api/Decks/${deck.id}`);

      if (response.data.deck.iconPath.length === 0){
        response.data.deck.iconPath = deckIcon;
      }
      
      setDeck(response.data.deck);
    } catch (error) {
      console.error('Error fetching data: ', error);
    }
  }

  async function addCardRequest(primaryText, secondaryText) {
    const request = {
      deckId: deck.id,
      primaryText: primaryText,
      secondaryText: secondaryText
    };
    try {
      await axios.put(`${api_URL}/api/Cards`, request);
    } catch (error) {
      console.error("Error sending data: ", error);
    }
  }

  async function deleteDeck(id){
    try{
      await axios.delete(`${api_URL}/api/Decks/${id}`);
      window.location.assign('/');
    }
    catch(error){
      console.error("Error deleting deck: ", error);
    }
  }

  function addCardForm(){
    const handleAddCard = async () => {
      const primaryText = document.querySelector('#primary-word');
      const secondaryText = document.querySelector('#secondary-word');
      await addCardRequest(primaryText.value, secondaryText.value);

      primaryText.value = '';
      secondaryText.value = '';
      await updateCardsData();
    };

    return (
      <div id="add-card-form">
        <li className="list-group-item "> 
          <div className="row border">
            <div className="col-10">
              <div className="row"><input id="primary-word" type="text" className="form-control" placeholder="Word" aria-label="Primary word" /></div>
              <div className="row"><input id="secondary-word" type="text" className="form-control" placeholder="Word Translation" aria-label="Secondary word" /></div>
            </div>
            <div onClick={handleAddCard} className="col-2 bg-light btn position-relative"> {/* Исправлено class на className */}
              <i className="bi bi-plus-square position-absolute top-50 start-50 translate-middle"></i> {/* Исправлено class на className */}
            </div>
          </div>
        </li>
      </div>
    );
  }

  function cardsView(cards) {
    let isAuthor = false;
    if (user?.role.toLowerCase() === "teacher" && user?.id === deck.author.id) {
      isAuthor = true;
    }
    const cardList = cards.map(function (card) {
      return (
        <li key={card.id} className="list-group-item mt-1 mb-1"> 
          <div className='row'>
            <div className='col'>
              <div className="btn bg-light bg-gradient border">{card.primaryText}</div> 
              <div className="btn bg-light bg-gradient border">{card.secondaryText}</div> 
            </div>
            {isAuthor &&
              <div onClick={() => { deleteCard(card.id) }} className='btn btn-danger col-auto'>
                <i className="bi bi-trash3" />
              </div>
            }
          </div>
        </li>
      );
    });

    return (
      <ul className="list-group text-center"> 
        <li key="CardsCount" className="list-group-item active border-bottom bg-info" aria-current="true">Cards ({cards.length})</li> {/* Исправлено class на className */}

        <div id="cartsList">
          {isAuthor && addCardForm()}
          <div>{cardList}</div>
        </div>
      </ul>
    );
  }

  function deckInfoView() {
    return (
      <div id="deckInfo" className="col-md-6 mt-3 "> 
        <div id="deck-icon" className="bg-light border border-primary-subtle border-3 rounded"> 
          <img src={deck.iconPath} alt="Deck Icon" className="mx-auto d-block" /> 
        </div>
        <div>
          <div className="align-items-center text-center row mt-2"> 
            <div className="col-6">
              <div className="text-center fs-5">{deck.name}</div>
            </div>
            <div className="col-6">
              <div className="row ">
                <div className="text-center ">{deckRating.rating.toFixed(1)}<i className="bi bi-star"></i></div> 
              </div>
              <div className=" row">
                <div className="fst-italic">{deckRating.votes + " votes"}</div>
              </div>
            </div>
          </div>

          <div className="align-items-center text-center row ms-1"> 
            <div className="col">
              <div className="row">Original language: {deck.primaryLanguage}</div>
              <div className="row">Translation language: {deck.secondaryLanguage}</div>
              <div className="row">Category: {deck.category?.name}</div>
            </div>
            <div className="col">
              <div className='btn text-primary'>{deck.author.name}</div>
            </div>
          </div>

          <div className="align-items-center text-center row "> 
            <div className="w-100 d-none d-md-block">Description</div>
            <div className="col">{deck.description}</div>
          </div>

          <div className="align-items-center text-center row "> 
            <div className="col">{interactiveElements(user, deck)}</div>
          </div>

        </div>
      </div>
    );
  }

  function interactiveElements() {

    if (user?.role.toLowerCase() === "student" || (user?.role.toLowerCase() === "teacher" && user?.id !== deck.author.id)) {
      return (
        <div>
          <div className='row mx-1 mt-1'>
            <div className='row align-items-center text-center'><div>Progress</div></div>
            <div className='d-flex'>
              <ProgressBar progress={progress} />
            </div>

          </div>
          <div className="row">
            <div className="col mt-1">
              <Link to={`/decks/${deck.id}/cards`} key={deck.id}>
                <div className="border btn bg-primary text-white">Play</div> {/* Исправлено class на className */}
              </Link>
            </div>
          </div>
        </div>

      );
    }

    else if (user?.role.toLowerCase() === "teacher") {
      return (
        <div>
          <div className='row'>
            <ProgressBar progress={progress} />
          </div>
          <div className="row">
            <div className="row">
              <div className="col mt-1">
                <Link
                  to={`/decks/${deck.id}/edit`}
                  key={deck.id}
                  state={{ key: deck.id, deck: deck }}>
                  <div className="border btn bg-primary text-white">Edit</div> {/* Исправлено class на className */}
                </Link>
              </div>
              <div class = "col mt-1">
                <div class = "border btn bg-primary text-white" onClick={() => setModalVisible(true)}>Delete</div>
              </div>
            </div>
            <div className="row">
              <div className="col mt-1">
                <Link to={`/decks/${deck.id}/cards`} key={deck.id}>
                  <div className="border btn bg-primary text-white">Play</div> {/* Исправлено class на className */}
                </Link>
              </div>
            </div>
          </div>
          <DeleteDeckModal
            show={modalVisible}
            onHide={() => setModalVisible(false)}
            onConfirm={() => deleteDeck(deck.id)}
          />
        </div>

      );
    }

    else {
      return (
        <div>Sign in to play this deck</div>
      );
    }
  }

  async function DeleteComment(commentId) {
    
    try {
      var url = `${api_URL}/api/Comments?commentId=${commentId}&userId=${user.id}&deckId=${deck.id}`;
      await axios.delete(url);
      props.SetUpdate();
    } catch (errorMessage) {
      if (errorMessage.response) {
        setError(errorMessage.response.data.Details);
      } else {
        setError(errorMessage.message);
      }
    }
  }

  function commentsView() {
    const commentList = comments.map(function (comment) {
      return (
        <li key={comment.id} className="list-group-item mt-1 mb-1">
          <div className="row">
            <div className="col-auto">
              <img src={comment.user.iconPath} alt="Avatar" width="32" />
            </div>
            <div className="col-auto fs-5 me-auto">{comment.user.name}</div>
            <div className="col-auto">{comment.date}</div>
            {((user?.role.toLowerCase() === "teacher" && deck.author?.id === user.id) || comment.user.id === user.id) ||
              (user?.role.toLowerCase() === "student" && comment.user?.id === user.id) ||
              (user?.role.toLowerCase() === "admin" || comment.user?.id === user.id) ? (
              <div className="col-auto btn btn-danger" onClick={() => DeleteComment(comment.id)}>🗑</div>
            ) : null}
          </div>
          <div className="row justify-content-start text-start">{comment.text}</div>
        </li>
      );
    });

    return (
      <ul className="list-group text-center"> 
        <li className="list-group-item active border-bottom bg-info" aria-current="true">Comments ({comments.length})</li> {/* Исправлено class на className */}
        <div id="commentList">{commentList}</div>
        <div className="mb-4">
        <Comment userId={user.id} deckId={deck.id} SetUpdate={props.SetUpdate}/>
          {error ? (
            <div className="alert alert-danger text-center" role="alert">{error}</div>
          ) : null}
        </div>
      </ul>
    );
  }

  return (
    <div id="content" className="container bg-primary-subtle bg-gradient"> 
      <div id="deckDisplay" className="row"> 
        <div id="displayCards" className="col-md-6 mt-3">{cardsView(deck.cards)}</div> 
        {deckInfoView()}
        <div className="w-100 d-none d-md-block" /> 
        <div className="col mt-3 border">
          {user && user.id ? commentsView() : null}
        </div>
      </div>
    </div>
  );
}

export default DeckPage;
