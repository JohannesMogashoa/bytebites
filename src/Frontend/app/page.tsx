import Hero from "@/components/client/HeroComponent";
import { RecipeListingPageComponent } from "@/components/client/RecipeListingPageComponent";

export default async function HomePage() {
  return (
    <section>
      <Hero />
      <RecipeListingPageComponent />
    </section>
  );
}
