#ifndef STATICCLASS_H_INCLUDED
#define STATICCLASS_H_INCLUDED


#include "Enums.h"

#include <boost/thread/mutex.hpp>
#include<boost/thread/condition_variable.hpp>

#include <queue>



using namespace std;
using namespace Enums;

class concurrent_queue
{
private:
    std::queue<PFHolder> the_queue;
    mutable boost::mutex the_mutex;
    boost::condition_variable the_condition_variable;
    int _count;
public:
    concurrent_queue()
    {
        _count=0;
    }

    void push(PFHolder data)
    {
        boost::mutex::scoped_lock lock(the_mutex);
        the_queue.push(data);

        if(the_queue.empty())
        {
            cout << " Test Print :: Queue is blank "<<endl<<endl;
        }

        _count++;
        lock.unlock();
        the_condition_variable.notify_one();
    }

    int size() const
    {
        boost::mutex::scoped_lock lock(the_mutex);
        return _count;
    }

    bool empty() const
    {
        boost::mutex::scoped_lock lock(the_mutex);
        return the_queue.empty();
    }

    PFHolder try_pop()
    {
        PFHolder popped_value;
        memset(&popped_value,0,sizeof(PFHolder));
        boost::mutex::scoped_lock lock(the_mutex);
        if(the_queue.empty())
        {
            popped_value.PF=-1;
            return popped_value;
        }

        popped_value=the_queue.front();
        _count--;
        the_queue.pop();
        return popped_value;
    }

    void wait_and_pop(PFHolder popped_value)
    {
        boost::mutex::scoped_lock lock(the_mutex);
        while(the_queue.empty())
        {
            the_condition_variable.wait(lock);
        }

        popped_value=the_queue.front();
        the_queue.pop();
    }

};
  extern concurrent_queue _Bidqueue;
  extern concurrent_queue _Mktqueue;



  static class FillData
    {

  public:

 //boost::lockfree::queue<char*> _Bidqueue;
 //boost::lockfree::queue<char*> _Mktqueue;

      void BidQueue(PFHolder obj)
      {
           _Bidqueue.push(obj);
      }
      void MktQueue(PFHolder obj)
      {
           _Mktqueue.push(obj);
      }

    int _BidQueuesize()
    {
        return _Bidqueue.size();
    }

   int _MktQueuesize()
    {
        return _Mktqueue.size();
    }

    PFHolder _GetBidData()
    {
        return _Bidqueue.try_pop();
    }

    PFHolder _GetMktData()
    {
        return _Mktqueue.try_pop();
    }

    }_FillData;



template <class T>
class MyHash;

template<>
class MyHash<StructHash>
{
public:
    std::size_t operator()(StructHash const& s) const
    {
        std::size_t h1 = std::hash<int>()(s.Token1);
        std::size_t h2 = std::hash<int>()(s.Token2);
        std::size_t h3 = std::hash<int>()(s.Token3);
        std::size_t h4 = std::hash<int>()(s.Price1);
        std::size_t h5 = std::hash<int>()(s.Price2);
        std::size_t h6 = std::hash<int>()(s.Price3);
        std::size_t h7 = std::hash<int>()(s.Qty1);
        std::size_t h8 = std::hash<int>()(s.Qty2);
        std::size_t h9 = std::hash<int>()(s.Qty3);
        std::size_t h10 = std::hash<short>()(s.side1);
        std::size_t h11 = std::hash<short>()(s.side2);
        std::size_t h12 = std::hash<short>()(s.side3);
        return h1 ^ h2 ^ h3 ^ h4 ^ h5 ^ h6 ^ h7 ^ h8 ^ h9 ^ h10 ^ h11 ^  (h12 << 1);
    }
};
#endif // STATICCLASS_H_INCLUDED
