#include <iostream>
#include <thread>
#include <chrono>

bool isOK = false;

void mainT()
{
	while (true)
	{
		//サブを動作させる
		isOK = true;

		//ローディングに15びょうかかるとする
		std::this_thread::sleep_until(std::chrono::system_clock::now() + std::chrono::seconds(15));
		
		//ローディングが終わり次第表示
		std::cout << "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" << std::endl;

		//サブを無効に
		isOK = false;

		//終わり
		return;
	}
}

void subT()
{
	while (isOK)
	{
		std::this_thread::sleep_until(std::chrono::system_clock::now() + std::chrono::seconds(1));
		std::cout << "a" << std::endl;
	}
}

int main()
{
	std::thread main(mainT);
	std::thread sub(subT);
	main.join();
	sub.join();


	return 0;
}