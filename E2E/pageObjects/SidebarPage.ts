import { Page, Locator } from "playwright";

export class SidebarPage {
  public readonly page: Page;
  public readonly root: Locator;
  public readonly home: Locator;
  public readonly aperture: Locator;
  public readonly clickedHome: Locator;

  constructor(page: Page) {
    this.page = page;
    this.root = this.page.locator("//div[contains(@class,'sidebar')]");
    this.home = this.root.locator("//div//span[contains(@class,'oi-home')]");
    this.clickedHome= this.page.locator("//a[@aria-current='page']")
    this.aperture = this.root.locator(
      "//div//span[contains(@class,'oi-aperture')]"
    );
  }

  async isVisible(): Promise<boolean> {
    return this.root.isVisible();
  }

  async clickHome(): Promise<void> {
    await this.home.click();
  }

  async clickAperture(): Promise<void> {
    await this.aperture.click();
    }

}