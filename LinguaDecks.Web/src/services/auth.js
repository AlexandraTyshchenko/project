import axios from "axios";

const api_URL = process.env.REACT_APP_API_URL;
const authKey = 'auth';
const authScheme = 'Bearer';

axios.interceptors.request.use(r=>{
  if(isAuthorized()){
    const token = getAuth().accessToken;
    axios.defaults.headers.common['Authorization'] = `${authScheme} ${token}`;
  }
  return r;
});

export async function signInAsync(email, password) {
  const response = await axios.post(api_URL + '/api/Auth/signin', {email, password});
  localStorage.setItem(authKey, JSON.stringify({
    accessToken: response.data.token,
    refreshToken: {
      value: response.data.refreshToken.value,
      expires: response.data.refreshToken.expirationDate
    },
    user: response.data.refreshToken.user
  }));
  
  window.location.assign("/");
}

export async function signUpAsync(name, email, password, role, description) {
  await axios.post(api_URL + '/api/Auth/signup', {name, email, password, role, description});
  await signInAsync(email, password);
  window.location.assign("/");
}

export function signOut() {
  localStorage.removeItem(authKey);
  window.location.assign("/");
}

export function getAuth() {  
  return JSON.parse(localStorage.getItem(authKey));
}

export function isAuthorized() {
  return getAuth() !== null;
}

export async function refreshAsync() {
  const headers = {
    'Content-Type': 'application/json',
  };
  const data = await getAuth();
  const response = await axios.post(api_URL + '/api/Auth/refresh', data.accessToken, {headers});
  localStorage.setItem(authKey, JSON.stringify({
    accessToken: response.data.token,
    refreshToken: {
      value: response.data.refreshToken.value,
      expires: response.data.refreshToken.expirationDate
    },
    user: response.data.refreshToken.user
  }));
}