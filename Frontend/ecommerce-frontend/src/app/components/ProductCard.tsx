import { Product } from '@/app/types/product'
import Image from 'next/image'
import { Button } from './ui/button'
import { useAppDispatch } from '@/app/hooks/useRedux'
import { addToCart } from '@/app/features/cart/cartSlice'

interface Props {
  product: Product
}

export const ProductCard = ({ product }: Props) => {
  const dispatch = useAppDispatch()
  return (
    <div className="border rounded-xl p-4 shadow-sm space-y-2">
      <img
        src={product.imageUrl}
        alt={product.name}
        className="w-full h-48 object-cover rounded-md"
      />
      <h3 className="text-lg font-semibold">{product.name}</h3>
      <p className="text-gray-600 text-sm">{product.brand}</p>
      <p className="text-gray-800 font-bold">{product.price.toLocaleString()} تومان</p>
      <Button
        className="w-full mt-2"
        onClick={() => dispatch(addToCart(product))}
      >
        افزودن به سبد خرید
      </Button>
    </div>
  )
}