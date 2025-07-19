import { createSlice, createAsyncThunk } from '@reduxjs/toolkit'
import axios from '@/app/lib/axios'

interface LoginError {
    message: string
}

export const login = createAsyncThunk<any, { email: string; password: string }, { rejectValue: LoginError }>(
    'auth/login',
    async (data, thunkAPI) => {
        try {
            const res = await axios.post('/auth/login', data)
            return res.data
        } catch (err: any) {
            return thunkAPI.rejectWithValue({ message: err.response?.data?.message || 'Login Failed' })
        }
    }
)

const authSlice = createSlice({
    name: 'auth',
    initialState: {
        user: null,
        loading: false,
        error: null as string | null
    },
    reducers: {
        logout: (state) => {
            state.user = null
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(login.pending, (state) => {
                state.loading = true
                state.error = null
            })
            .addCase(login.fulfilled, (state, action) => {
                state.loading = false
                state.user = action.payload
            })
            .addCase(login.rejected, (state, action) => {
                state.loading = false
                state.error = action.payload?.message || 'Login Failed'
            })
    },
})

export const { logout } = authSlice.actions
export default authSlice.reducer