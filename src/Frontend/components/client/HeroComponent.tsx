import Link from "next/link";

export default function Hero() {
  return (
    <section className="bg-gradient-to-br from-orange-100 via-white to-yellow-50 px-4 py-20 sm:px-8 lg:px-16">
      <div className="mx-auto max-w-5xl text-center">
        <h1 className="mb-6 text-4xl leading-tight font-extrabold text-gray-900 sm:text-5xl">
          Discover. Cook. Share.
        </h1>
        <p className="mx-auto mb-8 max-w-2xl text-lg text-gray-600 sm:text-xl">
          Welcome to{" "}
          <span className="font-semibold text-orange-500">ByteBites</span> —
          where home cooks, food lovers, and bloggers come together to explore
          and share mouthwatering recipes.
        </p>
        <div className="flex flex-col justify-center gap-4 sm:flex-row">
          <Link
            href="/recipes"
            className="rounded-xl border border-orange-500 px-6 py-3 font-medium text-orange-500 transition hover:bg-orange-100"
          >
            Share Your Own
          </Link>
        </div>
      </div>
    </section>
  );
}
