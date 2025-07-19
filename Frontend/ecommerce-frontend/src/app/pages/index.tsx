import { useEffect, useState } from 'react'
import { getProducts } from '@/app/lib/api/catalog'
import { Product } from '@/app/types/product'
import { ProductCard } from '@/app/components/ProductCard'

export default function HomePage() {
  const [products, setProducts] = useState<Product[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    const fetchData = async () => {
      try {
        const res = await getProducts()
        setProducts(res)
      } catch (err) {
        setError('خطا در دریافت محصولات')
      } finally {
        setLoading(false)
      }
    }

    fetchData()
  }, [])

  if (loading) return <p className="text-center py-10">در حال بارگذاری...</p>
  if (error) return <p className="text-center text-red-500">{error}</p>

  return (
    <main className="max-w-7xl mx-auto p-4 grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
      {products.map((product) => (
        <ProductCard key={product.id} product={product} />
      ))}
    </main>
  )
}