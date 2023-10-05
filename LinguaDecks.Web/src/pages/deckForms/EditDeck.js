import { useLocation, useNavigate } from 'react-router-dom';
import axios from 'axios';
import DeckInfoForm from '../../components/deckInfoForm/DeckInfoForm';
import './DeckForms.css';

function EditDeck() {
  const location = useLocation();
  const deck = location.state.deck;

  const navigate = useNavigate();

  async function onSubmitHandler(values) {
    const api_URL = process.env.REACT_APP_API_URL;
    var request = {
      name: values.name,
      primaryLanguage: values.primaryLanguage,
      secondaryLanguage: values.secondaryLanguage,
      categoryId: parseInt(values.categoryId),
      description: values.description,
      icon: values.icon instanceof File ? values.icon : null
    };
    try {
      await axios.put(`${api_URL}/api/Decks/${deck.id}`, request, { headers: { 'Content-Type': 'multipart/form-data' }});
      navigate(-1);
    }
    catch (error) {
      console.error('Error updating data: ', error);
    }
  };

  return (
    <div id="content" class="container bg-primary-subtle bg-gradient">
    <div class='d-flex justify-content-center mt-3'>
      <h2 class='mb-0'>Edit deck</h2>
    </div>
      <DeckInfoForm deck={deck} onSubmitHandler={onSubmitHandler}/>
    </div>
  );
};

export default EditDeck;