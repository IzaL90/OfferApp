import { expect } from "@playwright/test";
import { Locator } from "playwright";

export class EditComponent {
  public readonly root: Locator;
  public readonly name: Locator
  public readonly price: Locator
  public readonly description:Locator
  public readonly submit: Locator
  public readonly cancel: Locator

  constructor(root: Locator){
    this.root = root;
    this.name= this.root.locator("//input[@data-name='bid-name-input']");
    this.price = this.root.locator("//input[@data-name='bid-first-price-input']")
    this.description = this.root.locator("//textarea[@data-name='bid-description-input']")
    this.submit = this.root.locator("//button[@data-name='bid-submit-button']")
    this.cancel = this.root.locator("//button[@data-name='bid-close-button']")
  }

  public async isVisible(): Promise<boolean> {
    return this.root.isVisible();
  }
  
  public async expectModalVisible(): Promise<void> {
    await expect(this.name).toBeVisible()
    await expect(this.price).toBeVisible()
    await expect(this.description).toBeVisible()
    await expect(this.submit).toBeVisible()
    await expect(this.cancel).toBeVisible() 
  }

}