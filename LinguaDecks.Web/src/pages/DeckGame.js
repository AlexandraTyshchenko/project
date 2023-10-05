import React, { useEffect, useState } from 'react';
import axios from 'axios';
import ViewGame from './Game/ViewGame';
import Loading from './Loading';
import { useParams } from 'react-router-dom';
import { getAuth } from "../services/auth";

function DeckGame(props){
  const { id } = useParams();
  const [deck, setDeck] = useState(null);
  //set user id from auth data
  const userId = getAuth()?.user.id;
  
  const api_URL = process.env.REACT_APP_API_URL; 
  
  useEffect(() => {
    async function fetchData () {
			try {
				const response = await axios.get(`${api_URL}/api/decks/${id}/cards`);

        setDeck(response.data);        
			} 
			catch (error) {
				console.error('Error fetching data: ', error);
			}
		};
    fetchData();
  },[id, api_URL]);
  if (!deck || !userId) {
    return (
      <Loading />
    );
  }
  
  return(
    <ViewGame Deck = {deck} UserId = {userId}/>
  );
}

export default DeckGame;