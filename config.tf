variable "region" {
  default     = "us-east-1"
  description = "Region of AWS"
}

provider "aws" {
  region = var.region
}

data "aws_availability_zones" "available" {}

locals {
  cluster_name = "picturesocial-${random_integer.suffix.result}"
}

resource "random_integer" "suffix" {
  min = 100
  max = 999
}