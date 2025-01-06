import test, { expect, Page } from "@playwright/test";
import { Locator } from "playwright";
import { FormComponent } from "./FormComponent";
import { Bid, defaultBid } from "../interfaces/Bid";

export class AddComponent {
  public readonly root: Locator;
  public readonly submit: Locator;
  public readonly cancel: Locator;
  public readonly close: Locator;
  public readonly form: FormComponent;

  constructor(root: Locator) {
    this.root = root;
    this.submit = this.root.locator("//button[@data-name='bid-submit-button']");
    this.cancel = this.root.locator("//button[@data-name='bid-close-button']");
    this.close = this.root.locator("//button[@data-name='modal-close-button']");
    this.form = new FormComponent(this.root.locator("//form"));
  }

  public async isVisible(): Promise<boolean> {
    return this.root.isVisible();
  }

  public async expectModalVisible(): Promise<void> {
    await this.form.expectFormVisible();
    await expect(this.submit).toBeVisible();
    await expect(this.cancel).toBeVisible();
    await expect.soft(this.close).toBeVisible();
  }

  public async fillForm(bid: Bid = defaultBid): Promise<void> {
    await this.form.name.fill(bid.name);
    await this.form.price.fill(bid.price);
    await this.form.description.fill(bid.description);
  }

  public async clickSubmit(): Promise<void> {
    await this.submit.click();
  }
}