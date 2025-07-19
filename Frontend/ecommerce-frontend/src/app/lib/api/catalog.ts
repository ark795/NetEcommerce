import axios from '../axios'
import { Product } from '@/app/types/product'

export const getProducts = async (): Promise<Product[]> => {
  const response = await axios.get('/catalog/products')
  return response.data
}