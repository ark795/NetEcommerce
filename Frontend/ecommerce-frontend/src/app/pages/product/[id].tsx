import { useRouter } from 'next/router'
import { useEffect, useState } from 'react'
import { Product } from '@/app/types/product'
import axios from '@/app/lib/axios'
import { Button } from '@/app/components/ui/button'
import { useAppDispatch } from '@/app/hooks/useRedux'
import { addToCart } from '@/app/features/cart/cartSlice'

export default function ProductDetail() {
  const router = useRouter()
  const { id } = router.query
  const [product, setProduct] = useState<Product | null>(null)
  const dispatch = useAppDispatch()

  useEffect(() => {
    if (id) {
      axios.get(`/catalog/products/${id}`).then(res => {
        setProduct(res.data)
      })
    }
  }, [id])

  if (!product) return <p className="text-center py-20">در حال بارگذاری...</p>

  return (
    <div className="max-w-4xl mx-auto py-10 px-4 grid grid-cols-1 md:grid-cols-2 gap-8">
      <img src={product.imageUrl} alt={product.name} className="w-full h-96 object-cover rounded" />
      <div>
        <h2 className="text-2xl font-bold mb-2">{product.name}</h2>
        <p className="text-gray-600 mb-2">{product.brand}</p>
        <p className="mb-4">{product.description}</p>
        <p className="font-bold text-xl mb-4">{product.price.toLocaleString()} تومان</p>
        <Button onClick={() => dispatch(addToCart(product))}>افزودن به سبد خرید</Button>
      </div>
    </div>
  )
}