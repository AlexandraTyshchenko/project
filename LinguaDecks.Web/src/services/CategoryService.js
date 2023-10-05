import axios from 'axios';

const api_URL = process.env.REACT_APP_API_URL;

const CategoryService = {

    async getAll() {
        try {
            const response = await axios.get(`${api_URL}/api/Categories`);
            return response.data;
        } catch (error) {
            throw error;
        }
    }
    
};

export default CategoryService;
