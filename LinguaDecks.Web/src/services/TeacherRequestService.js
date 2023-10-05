import axios from 'axios';
import { getAuth } from './auth';
const api_URL = process.env.REACT_APP_API_URL;

const TeacherRequestService = {
    async getAll(currentPage, pageSize, status) {
        try {
            const url = `${api_URL}/api/TeacherRequests?page=${currentPage}&pageSize=${pageSize}&status=${status}`;
            const response = await axios.get(url);
            if (response.status === 200) {
                return {
                    teacherRequests: response.data.teacherRequests,

                    totalTeacherRequests: response.data.totalTeacherRequests
                };
            } else {
                throw new Error(`error ${response.status}`);
            }
        } catch (error) {
            throw error;
        }
    }
    ,

    async answerRequest(id, answer) {
        try {
            const url = `${api_URL}/api/TeacherRequests?id=${id}&answer=${answer}`;
            console.log(url);
            const response = await axios.post(url);
            if (response.status != 200) {
                throw new Error(`Error ${response.status}`);
            }
        } catch (error) {
            throw error;
        }
    }
};

export default TeacherRequestService;
