import React, { useEffect, useState } from 'react';
import axios from 'axios';
import ViewProfile from './ViewProfile/ViewProfile';
import Loading from './Loading';
import { getAuth } from '../services/auth';

function Profile(){
  const api_URL = process.env.REACT_APP_API_URL;
	const [totalProgress, setTotal] = useState(null);
	const [allProgresses, setAll] = useState(null);
  const [user, setUser] = useState(null);
  const [deckIcons, setIcons] = useState(null);

  const token = getAuth().accessToken;
  axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
  
  useEffect(() => {
    async function fetchData() {
      try {
        const [userResponse, totalResponse, allResponse] = await Promise.all([
          axios.get(`${api_URL}/api/profile`),
          axios.get(`${api_URL}/api/profile/statistics`),
          axios.get(`${api_URL}/api/profile/progress`)
        ]);
  
        setUser(userResponse.data);
        setTotal(totalResponse.data);
        setAll(allResponse.data);
      } 
      catch (error) {
        console.error('Error fetching data: ', error);
      }
    };

    fetchData();
  }, [api_URL]);
  
  useEffect(() => {
    async function fetchDataIcons() {
      if (!allProgresses) return;
  
      try {
        const deckIds = allProgresses.map(item => item.deckId);
        const iconsResponse = await axios.get(`${api_URL}/api/decks/icons`, {
          headers: { 'deckIds': deckIds.join(',') }
        });
  
        setIcons(iconsResponse.data);
      } 
      catch (error) {
        console.error('Error fetching icons: ', error);
      }
    };

    fetchDataIcons();
  }, [allProgresses]);

    if (!totalProgress || !allProgresses || !user) {
      return (
        <Loading />
      );
    }

    return(<ViewProfile TotalProgress = {totalProgress} AllProgresses = {allProgresses} User = {user} DeckIcons = {deckIcons}/>)
}

export default Profile;